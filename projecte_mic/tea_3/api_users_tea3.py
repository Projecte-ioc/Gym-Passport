import os
from dotenv import load_dotenv
import jwt
import psycopg2
from flask import Flask, request, jsonify
from werkzeug.security import generate_password_hash
from ..tea_2.database_models_tea2 import User
from ..tea_2.utils_tea_2 import Connexion

app = Flask(__name__)
db = Connexion()
load_dotenv()


def register(userObj, cursor):
    user_name_exists = db.get_elements_filtered(userObj.get_user_name(), "users_data", "user_name", '*')
    if not user_name_exists:
        try:
            cursor.execute("INSERT INTO users_data (name, rol_user, pswd_app, gym_id, user_name, log) VALUES ("
                           "%s, %s, %s, %s, %s, %s)", (userObj.get_name(),
                                                       userObj.get_rol_user(), userObj.get_pswd_app(),
                                                       userObj.get_gym_id(),
                                                       userObj.get_user_name(), userObj.get_log()
                                                       ))
        except psycopg2.Error as e:
            print(e)

    return 'Usuario ya existe'


@app.route('/profile_info', methods=['GET'])
def select_a_user_info_and_gym():
    connection, cursor = db.get_connection_to_db()
    token = request.headers.get('Authorization')
    rol, id, user_name, _ = db.validate_rol_user(token)
    if rol == 'admin':
        query = """
        SELECT users_data.name as user_name, users_data.rol_user, gym.name as gym_name, gym.address, gym.phone_number, gym.schedule
        FROM users_data
        JOIN gym ON users_data.gym_id = gym.id
        WHERE users_data.user_name = %s
        """
        cursor.execute(query, (user_name,))
        results = cursor.fetchone()
        connection.close()

        if results:
            column_names = [desc[0] for desc in cursor.description]
            formatted_record = dict(zip(column_names, results))
            print(type(jsonify(formatted_record).get_json()))
            token_ad = jwt.encode(formatted_record,
                                  os.getenv("SK"), algorithm='HS256')
            return jsonify({'ad-token': f'{token_ad}'})
        else:
            return jsonify({'error': 'No se encontraron registros para el usuario'})

    else:

        return jsonify({'nl-token': f'{token}'})


@app.route('/insert_client', methods=['POST'])
def insert_individual_client():
    connection, cursor = db.get_connection_to_db()
    token = request.headers.get('Authorization')
    rol_user, id, _, _ = db.validate_rol_user(token)
    data = request.get_json(force=True)
    try:
        if isinstance(data, dict):
            name = data.get('name')
            rol = data.get('rol_user')
            user = data.get('user_name')
            pswd = generate_password_hash(data.get('pswd_app'), method='pbkdf2', salt_length=16)
        else:
            for item in data:
                name = item['name']
                rol = item['rol_user']
                user = item['user_name']
                pswd = generate_password_hash(item['pswd_app'], method='pbkdf2', salt_length=16)
        if rol_user == 'admin':
            user = User(id=_, name=name, rol_user=rol, pswd_app=pswd, gym_id=id, user_name=user, log=0)
            register(user, cursor)

            connection.commit()
            cursor.close()
            connection.close()

            return jsonify({'message': 'Usuario registrado correctamente'})
    except psycopg2.Error as e:
        return jsonify(
            {'message': f'el usuario no se ha podido registrar porque ya existe o por falta de permisos {e}'}), 500
    finally:
        cursor.close()
        connection.close()


@app.route('/insert_some_clients', methods=['POST'])
def insert_diferents_clients():
    connection, cursor = db.get_connection_to_db()
    token = request.headers.get('Authorization')
    rol_user, id, _, _ = db.validate_rol_user(token)
    data = request.get_json(force=True)
    if rol_user == 'admin':
        try:
            for item in data:
                # obtiene del json
                name = item['name']
                rol = item['rol_user']
                user = item['user_name']
                pswd = generate_password_hash(item['pswd_app'], method='pbkdf2', salt_length=16)
                # crea un nuevo objeto del tipo usuario
                user = User(id=_, name=name, rol_user=rol, pswd_app=pswd, gym_id=id, user_name=user)
                register(user, cursor)
                connection.commit()
            return jsonify({'message': 'Usuario registrado correctamente'}), 200
        except psycopg2.Error as e:
            return jsonify({'message': f'Error {e}'}), 401
        finally:
            cursor.close()
            connection.close()
    return jsonify({'message': 'el usuario no se ha podido registrar porque ya existe o por falta de permisos'}), 500


@app.route('/update_data_client', methods=['PUT'])
def update_client_data():
    """
    Estructura JSON:
    {
        "name": "",
        "pswd_app": "",
        "rol_user": "",
        "user_name": ""
    }
    """
    connection, cursor = db.get_connection_to_db()
    token = request.headers.get('Authorization')
    rol_user, id, user_name, _ = db.validate_rol_user(token)
    data = request.get_json(force=True)
    try:
        if isinstance(data, dict):
            new_name = User.set_name(data.get('name'))
            new_rol = User.set_rol_user(data.get('rol_user'))
            user = data.get('user_name')
            if len(data.get('pswd_app')) < 20:
                new_pswd = generate_password_hash(data.get('pswd_app'), method='pbkdf2', salt_length=16)
            else:
                new_pswd = data.get('pswd_app')
        else:
            for item in data:
                new_name = item['name']
                new_rol = item['rol_user']
                user = item['user_name']
                if len(data.item['pswd_app']) < 20:
                    new_pswd = generate_password_hash(data.item['pswd_app'], method='pbkdf2', salt_length=16)
                else:
                    new_pswd = data.item['pswd_app']

        if rol_user == 'admin' or rol_user != 'admin' and user_name == user:
            if not user or (not new_name and not new_rol and not new_pswd):
                return "Faltan datos requeridos", 400
            cursor.execute("SELECT COUNT(*) FROM users_data WHERE user_name = %s", (user,))
            count = cursor.fetchone()[0]
            if count == 0:
                connection.close()
                return "No existe este usuario", 404
            # Realizar la actualización en la base de datos
            update_query = "UPDATE users_data SET"
            update_values = []

            if new_name:
                update_query += " name = %s,"
                update_values.append(new_name)

            if new_rol:
                update_query += " rol_user = %s,"
                update_values.append(new_rol)

            if new_pswd:
                update_query += " pswd_app = %s,"
                update_values.append(new_pswd)

            update_query = update_query.rstrip(',') + " WHERE user_name = %s"
            update_values.append(user)

            cursor.execute(update_query, tuple(update_values))
            return jsonify({'message': 'Datos actualizados correctamente'}), 200
        else:
            return jsonify({'message': 'No es poden actualitzar les dades per falta de permisos'}), 401
    except psycopg2.Error as e:
        print(e)
        return jsonify({'message': f'Error {e}'}), 401
    finally:
        connection.commit()
        connection.close()


@app.route('/delete_client?user_name={user}', methods=['DELETE'])
def delete_user():
    """
    :returns message of state endpoint connection
    En este caso, se tiene que pasar el 'user_name' del cliente.
    http://10.2.190.11:2000/delete_user?user_name=sonia33usr
    """
    user = request.args.get('user_name')
    token = request.headers.get('Authorization')
    rol_user, id, user_name, _ = db.validate_rol_user(token)
    if not user:
        return "Falta el nombre de usuario", 400
    if rol_user == 'admin':
        connex, cursor = db.get_connection_to_db()
        respuesta = db.get_elements_filtered(user, "users_data", "user_name", "id")
        if respuesta:
            # Realizar la eliminación en la base de datos
            cursor.execute("DELETE FROM users_data WHERE user_name = %s AND gym_id = %s", (user, id))
            connex.commit()
            connex.close()
            return jsonify({'message': 'Registro eliminado'}), 200  # Código de estado 200 OK

        else:
            return jsonify({'message': 'No existe ese registro'}), 404
    else:
        return jsonify({'message': 'No se ha podido llevar a cabo por falta de permisos'}), 401


if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=3000)
