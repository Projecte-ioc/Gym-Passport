import json

import jwt
import psycopg2
from flask import Flask, request, jsonify
from werkzeug.security import generate_password_hash, check_password_hash
import flask_wtf
import utils

app = Flask(__name__)

# Conexión a la base de datos PostgreSQL
db = utils.Connexion()

# Clave secreta para JWT
app.config['SECRET_KEY'] = db.SK


# Ruta para el registro de usuarios
@app.route('/register', methods=['POST'])
def register():
    connection, cursor = db.get_connection_to_db()
    data = request.get_json()
    for item in data:
        name = item['nombre']
        grants = item['permisos']
        user = item['usuario']
        print(str(len(user)))
        pswd = generate_password_hash(item['pswd_app'], method='pbkdf2', salt_length=16)
        print(str(len(pswd)))
        try:
            cursor.execute("INSERT INTO clientes (nombre, permisos, usuario, pswd_app) VALUES (%s, %s, %s, %s)", (name,
                                                                                                                  grants,
                                                                                                                  user,
                                                                                                                  pswd))
            connection.commit()
            return jsonify({'message': 'Usuario registrado correctamente'})
        except psycopg2.Error as e:
            return jsonify({'message': f'Error al registrar el usuario {e}'}), 500
        finally:
            cursor.close()
            connection.close()


# Ruta para la autenticación
@app.route('/login', methods=['POST'])
def login():
    data = request.get_json()
    print(type(data))
    connection, cursor = db.get_connection_to_db()
    for item in data:
        user = item["usuario"]
        pswd = item["pswd_app"]
        grants = item ["permisos"]

        try:
            cursor.execute("SELECT usuario, pswd_app, permisos FROM clientes WHERE usuario = %s", (user,))
            row = cursor.fetchone()
            if row and check_password_hash(row[1], pswd):
                token = jwt.encode({'username': user, 'pswd':pswd, 'grants' : grants}, app.config['SECRET_KEY'], algorithm='HS256')
                return jsonify({'token': token})
            return jsonify({'message': 'Credenciales inválidas'}), 401
        except psycopg2.Error as e:
            return jsonify({'message': f'Error en la autenticación {e}'}), 500
        finally:
            cursor.close()
            connection.close()


if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=4000)
