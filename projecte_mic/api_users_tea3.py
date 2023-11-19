import os
from dotenv import load_dotenv
import jwt
import psycopg2
from flask import Flask, request, jsonify
from werkzeug.security import generate_password_hash
from database_models_tea2 import User, Gym
from utils_tea_2 import Connexion

app = Flask(__name__)
db = Connexion()
load_dotenv()


def register(userObj, cursor):
    '''
    params: userObjet, cursor
    return: missatge en cas de que el usuari ja existeixi
    funció que s'encarrega d'afegir un nou usuari dins de la taula users_data
    '''
    user_name_exists = db.get_elements_filtered(userObj.user_name, userObj.__table_name__, "user_name", '*')
    if not user_name_exists:
        try:
            cursor.execute("INSERT INTO users_data (name, rol_user, pswd_app, gym_id, user_name, log) VALUES ("
                           "%s, %s, %s, %s, %s, %s)", (userObj.name,
                                                       userObj.rol_user, userObj.pswd_app,
                                                       userObj.gym_id,
                                                       userObj.user_name, userObj.log
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
        query = f"""
        SELECT users_data.name as user_name, users_data.rol_user, gym.name as gym_name, gym.address, gym.phone_number, gym.schedule
        FROM {User.__table_name__}
        JOIN gym ON users_data.gym_id = gym.id
        WHERE users_data.user_name = %s
        """
        cursor.execute(query, (user_name,))
        results = cursor.fetchone()
        connection.close()

        if results:
            column_names = [desc[0] for desc in cursor.description]
            formatted_record = dict(zip(column_names, results))
            token_ad = jwt.encode(formatted_record,
                                  os.getenv("SK"), algorithm='HS256')
            return jsonify({'ad-token': f'{token_ad}'}), 200
        else:
            return jsonify({'error': 'No ha estat possible recuperar els registres pel usuari'}), 404

    else:

        return jsonify({'nl-token': f'{token}'}), 200


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

            return jsonify({'message': 'Usuari enregistrat correctament'}), 201
    except psycopg2.Error as e:
        print(e)
        return jsonify(
            {'message': f'usuari no se ha enregistrat perque ja existeix o bé, per falta de permissos'}), 401
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
                user = User(id=_, name=name, rol_user=rol, pswd_app=pswd, gym_id=id, user_name=user,log=_)
                register(user, cursor)
                connection.commit()
            return jsonify({'message': 'Usuari enregistrat correctament'}), 200
        except psycopg2.Error as e:
            return jsonify({'message': f'Error {e}'}), 401
        finally:
            cursor.close()
            connection.close()
    return jsonify({'message': 'no es possible registrar perque ja existeix o bé, per falta de permissos'}), 401


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
            User.name = data.get('name')
            # new_name = User.set_name
            User.rol_user = data.get('rol_user')
            # new_rol = User.set_rol_user
            user = data.get('user_name')
            if len(data.get('pswd_app')) < 20:
                new_pswd = generate_password_hash(data.get('pswd_app'), method='pbkdf2', salt_length=16)
            else:
                new_pswd = data.get('pswd_app')
        else:
            for item in data:
                User.name = item['name']
                User.rol_user = item['rol_user']
                user = item['user_name']
                if len(data.item['pswd_app']) < 20:
                    new_pswd = generate_password_hash(data.item['pswd_app'], method='pbkdf2', salt_length=16)
                else:
                    new_pswd = data.item['pswd_app']

        if rol_user == 'admin' or rol_user != 'admin' and user_name == user:
            if not user or (not User.name and not User.rol_user and not new_pswd):
                return "Faltan datos requeridos", 400
            cursor.execute(f"SELECT * FROM {User.__table_name__} WHERE user_name = %s", (user,))
            count = cursor.fetchone()[0]
            if count == 0:
                connection.close()
                return "No existe este usuario", 404
            # Realizar la actualización en la base de datos
            update_query = f"UPDATE {User.__table_name__} SET"
            update_values = []

            if User.name:
                update_query += " name = %s,"
                update_values.append(User.name)

            if User.rol_user:
                update_query += " rol_user = %s,"
                update_values.append(User.rol_user)

            if new_pswd:
                update_query += " pswd_app = %s,"
                User.pswd_app = new_pswd
                update_values.append(User.pswd_app)

            update_query = update_query.rstrip(',') + " WHERE user_name = %s"
            update_values.append(user)

            cursor.execute(update_query, tuple(update_values))
            if user_name == user:
                new_token = jwt.encode({
                    'user_name': user,
                    'rol_user': User.rol_user,
                    'gym_name': db.get_elements_filtered(id, Gym.__table_name__, "id", "name")[0][0].replace(
                        "-", " "),
                    'name': User.name
                }, os.getenv('SK'), algorithm='HS256')
                return jsonify({'new_token': new_token}), 201
            return jsonify({'message': 'dades actualitzades correctament'})
        else:
            return jsonify({'message': 'No es poden actualitzar les dades per falta de permissos'}), 401
    except psycopg2.Error as e:
        print(e)
        return jsonify({'message': f'Error {e}'}), 401
    finally:
        connection.commit()
        connection.close()


@app.route('/delete_client', methods=['DELETE'])
def delete_user():
    """
    :returns message of state endpoint connection
    En este caso, se tiene que pasar el 'user_name' del cliente.
    http://10.2.190.11:2000/delete_user?user_name=sonia33usr
    """
    username = request.args.get('user_name')
    token = request.headers.get('Authorization')
    rol_user, id, user_name, _ = db.validate_rol_user(token)
    if not username:
        return "Falta el nombre de usuario", 400
    if rol_user == 'admin':
        connex, cursor = db.get_connection_to_db()
        respuesta = db.get_elements_filtered(username, User.__table_name__, "user_name", "id")
        if respuesta:
            # Realizar la eliminación en la base de datos
            cursor.execute(f"DELETE FROM {User.__table_name__} WHERE user_name = %s AND gym_id = %s", (username, id))
            connex.commit()
            connex.close()
            return jsonify({'message': 'Registre eliminat correctament'}), 201  # Código de estado 200 OK

        else:
            return jsonify({'message': 'No existeix aquest registre'}), 404
    else:
        return jsonify({'message': 'No es possible portar a terme per falta de permissos'}), 401


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=3000)
