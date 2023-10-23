from typing import Any

from flask import Flask, request, jsonify
import psycopg2

app = Flask(__name__)
db_params = {
    'dbname': 'gympassportdb',
    'user': 'isard',
    'password': 'pirineus',
    'host': '127.0.0.1',
    'port': '5432'
}


def get_connection_to_db():
    connex = psycopg2.connect(**db_params)
    cursor = connex.cursor()
    return connex, cursor


@app.route('/clientes', methods=['GET'])
def get_all_clientes():
    connex, cursor = get_connection_to_db()
    cursor.execute('SELECT * FROM clientes')
    registros = cursor.fetchall()
    connex.close()
    response = jsonify(registros)
    return response


@app.route('/cliente', methods=['GET'])
def get_clients_with_params():
    connex, cursor = get_connection_to_db()

    filter_name = request.args.get('nombre')
    filter_id = request.args.get('id')

    query_sql = "SELECT * FROM clientes WHERE 1=1"
    if filter_id:
        query_sql += f"AND id = '{filter_id}'"
    if filter_name:
        query_sql += f"AND nombre = '{filter_name}'"
    cursor.execute(query_sql)
    registros = cursor.fetchall()
    connex.close()

    response = jsonify(registros)
    return response


@app.route('/addcliente', methods=['POST'])
def add_client():

    nombre = request.args.get('nombre')
    permisos = request.args.get('permisos')
    usuario = request.args.get('usuario')
    pswd_app = request.args.get('pswd_app')

    if not nombre or not permisos or not usuario or not pswd_app:
        return "Faltan datos", 400
    connex, cursor = get_connection_to_db()
    cursor.execute("SELECT COUNT(*) FROM clientes WHERE usuario = %s", (usuario))
    count = cursor.fetchone()[0]
    if count > 0:
        connex.close()
        return "El usuario ya existe", 409

    cursor.execute("INSERT INTO clientes (nombre, permisos, usuario, pswd_app) VALUES (%s, %s, %s, %s)",
                   (nombre, permisos, usuario, pswd_app))
    connex.commit()
    connex.close()

    return "Registro creado", 201

@app.route('/updatecliente', methods=['PUT'])
def update_client():
    usuario = request.args.get('usuario')
    nuevo_nombre = request.args.get('nombre')
    nuevos_permisos = request.args.get('permisos')
    nuevo_password = request.args.get('pswd_app')

    if not usuario or (not nuevo_nombre and not nuevos_permisos and not nuevo_password):
        return "Faltan datos requeridos", 400

    connex, cursor = get_connection_to_db()

    # Verificar si el usuario existe
    cursor.execute("SELECT COUNT(*) FROM clientes WHERE usuario = %s", (usuario,))
    count = cursor.fetchone()[0]

    if count == 0:
        connex.close()
        return "El usuario no existe", 404  # Código de estado 404 Not Found

    # Realizar la actualización en la base de datos
    update_query = "UPDATE clientes SET"
    update_values = []

    if nuevo_nombre:
        update_query += " nombre = %s,"
        update_values.append(nuevo_nombre)

    if nuevos_permisos:
        update_query += " permisos = %s,"
        update_values.append(nuevos_permisos)

    if nuevo_password:
        update_query += " pswd_app = %s,"
        update_values.append(nuevo_password)

    update_query = update_query.rstrip(',') + " WHERE usuario = %s"
    update_values.append(usuario)

    cursor.execute(update_query, tuple(update_values))
    connex.commit()
    connex.close()

    return "Registro actualizado", 200  # Código de estado 200 OK


@app.route('/deletecliente', methods=['DELETE'])
def delete_client():
    id = request.args.get('id')

    if not id:
        return "Falta el identificador del usuario", 400

    connex, cursor = get_connection_to_db()

    # Verificar si el usuario existe
    cursor.execute("SELECT COUNT(*) FROM clientes WHERE usuario = %s", (usuario,))
    count = cursor.fetchone()[0]

    if count == 0:
        connex.close()
        return "El usuario no existe", 404  # Código de estado 404 Not Found

    # Realizar la eliminación en la base de datos
    cursor.execute("DELETE FROM clientes WHERE usuario = %s", (usuario,))
    connex.commit()
    connex.close()

    return "Registro eliminado", 200  # Código de estado 200 OK


if __name__ == '__main__':
    app.run(host="0.0.0.0", port=3000)
