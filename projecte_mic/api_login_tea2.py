import os
import jwt
import psycopg2
from flask import Flask, request, jsonify
from werkzeug.security import check_password_hash
from utils_tea_2 import Connexion
from database_models_tea2 import User
from dotenv import load_dotenv

app = Flask(__name__)

load_dotenv()

# Conexión a la base de datos PostgreSQL
db = Connexion()

# Clave secreta para JWT
app.config['SECRET_KEY'] = os.getenv("SK")


# Ruta para la autenticación
def get_user_by_user_name(user_name, pswd):
    connection, cursor = db.get_connection_to_db()

    try:
        cursor.execute("SELECT * FROM users_data WHERE user_name = %s", (user_name,))
        row = cursor.fetchone()
        if row and check_password_hash(row[3], pswd):
            return User(id=row[0], name=row[1], rol_user=row[2], pswd_app=row[3], gym_id=row[4], user_name=row[5],
                        log=row[6])
    except psycopg2.Error as e:
        print(f"Error: {e}")
    finally:
        cursor.close()
        connection.close()

    return None


@app.route('/login', methods=['POST'])
def login():
    data = request.get_json(force=True)
    connection, cursor = db.get_connection_to_db()
    if isinstance(data, dict):
        user_name = data.get("user_name")
        pswd = data.get("pswd_app")
    else:
        for item in data:
            user_name = item["user_name"]
            pswd = item["pswd_app"]

    if not user_name or not pswd:
        return jsonify({'message': 'Revisa que los campos no esten vacíos'}), 404
    user = get_user_by_user_name(user_name, pswd)
    if user:
        token = jwt.encode({
            'user_name': user.get_user_name(),
            'rol_user': user.get_rol_user(),
            'gym_name': db.get_elements_filtered(user.get_gym_id(), "gym", "id", "name")[0][0].replace("-", " "),
            'name': user.get_name()
        }, app.config['SECRET_KEY'], algorithm='HS256')
        if token:
            new_log = user.set_log(1)
            cursor.execute(f"UPDATE users_data SET log = {new_log} WHERE user_name = '{user.get_user_name()}'")
            connection.commit()
            connection.close()

        return jsonify({'token': token})
    else:
        return jsonify({'message': 'Credenciales inválidas'}), 401


@app.route('/logout', methods=['PATCH'])
def logout():
    connection, cursor = db.get_connection_to_db()
    token = request.headers.get('Authorization')
    _, _, user_name, _ = db.validate_rol_user(token)

    cursor.execute("SELECT * FROM users_data WHERE user_name = %s", (user_name,))
    row = cursor.fetchone()

    if row:
        # Crear el objeto User utilizando los valores recuperados de la base de datos
        user = User(id=row[0], name=row[1], rol_user=row[2], pswd_app=row[3], gym_id=row[4], user_name=row[5], log=row[6])

        # Establecer el log en 0
        new_log = user.set_log(0)

        # Actualizar el valor de log en la base de datos
        cursor.execute(f"UPDATE users_data SET log = {new_log} WHERE user_name = '{user_name}'")
        connection.commit()
        connection.close()
        return jsonify({'message': 'Cerrada la sessión correctamente.'}), 201
    else:
        return jsonify({'message': 'Usuario no encontrado o token inválido.'}), 404


if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=4000)
