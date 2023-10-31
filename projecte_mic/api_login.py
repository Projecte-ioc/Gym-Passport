import json

import jwt
import psycopg2
from flask import Flask, request, jsonify
from werkzeug.security import check_password_hash
import flask_wtf
import utils

app = Flask(__name__)

# Conexión a la base de datos PostgreSQL
db = utils.Connexion()

# Clave secreta para JWT
app.config['SECRET_KEY'] = db.SK


# Ruta para la autenticación
@app.route('/login', methods=['POST'])
def login():
    data = request.get_json(force=True)
    connection, cursor = db.get_connection_to_db()
    if isinstance(data, dict):
        user = data.get("user_name")
        pswd = data.get("pswd_app")
    else:
        for item in data:
            user = item["user_name"]
            pswd = item["pswd_app"]

    if not user or not pswd:
        return jsonify({'message': 'Revisa que los campos no esten vacíos'}), 404

    try:
        cursor.execute("SELECT name, pswd_app, rol_user, gym_id FROM users_data WHERE user_name = %s", (user,))
        row = cursor.fetchone()
        if row is not None:
            name = row[0]
            rol_user = row[2]
            gym_id = row[3]
            gym_name = db.get_elements_filtered(gym_id, "gym", "id", "name")[0][0].replace("-", " ")
            if row and check_password_hash(row[1], pswd):
                token = jwt.encode({'user_name': user, 'rol_user': rol_user, "gym_name": gym_name, "name": name},
                                   app.config['SECRET_KEY'], algorithm='HS256')
                return jsonify({'token': token})
        return jsonify({'message': 'Credenciales inválidas'}), 401
    except psycopg2.Error as e:
        return jsonify({'message': f'Error en la autenticación {e}'}), 500
    finally:
        cursor.close()
        connection.close()


if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=4000)
