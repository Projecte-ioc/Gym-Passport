from typing import Any

from flask import Flask, request, jsonify


import utils

app = Flask(__name__)
db = utils.Connexion()

def get_clients_with_par(filter):
    connex, cursor = db.get_connection_to_db()

    filter_name = request.args.get('nombre')
    filter_id = request.args.get('id')
    filter_user = request.args.get('usuario')

    query_sql = "SELECT * FROM clientes WHERE 1=1"
    if filter_id:
        query_sql += f" AND id = '{filter_id}'"
    if filter_name:
        query_sql += f" AND nombre = '{filter_name}'"
    if filter_user:
        query_sql += f" AND usuario = '{filter_user}'"
    cursor.execute(query_sql)
    records = cursor.fetchall()
    connex.close()

    return records


# DEVUELVE LOS VALORES EN JSON EN FORMATO LLAVE - VALOR
def format_records(records, column_names):
    formatted_records = []
    for record in records:
        formatted_record = {column_names[i]: record[i] for i in range(len(column_names))}
        formatted_records.append(formatted_record)
    return formatted_records


@app.route('/clientes', methods=['GET'])
def get_all_clientes():
    connex, cursor = db.get_connection_to_db()
    cursor.execute('SELECT * FROM clientes')
    records = cursor.fetchall()
    column_names = [desc[0] for desc in cursor.description]
    connex.close()
    formatted_records = format_records(records, column_names)
    return jsonify(formatted_records)


@app.route('/cliente', methods=['GET'])
def get_clients_with_params():
    connex, cursor = db.get_connection_to_db()

    filter_name = request.args.get('nombre')
    filter_id = request.args.get('id')
    filter_user = request.args.get('usuario')

    query_sql = "SELECT * FROM clientes WHERE 1=1"
    if filter_id:
        query_sql += f" AND id = '{filter_id}'"
    if filter_name:
        query_sql += f" AND nombre = '{filter_name}'"
    if filter_user:
        query_sql += f" AND usuario = '{filter_user}'"
    cursor.execute(query_sql)
    records = cursor.fetchall()
    column_names = [desc[0] for desc in cursor.description]
    connex.close()
    formatted_records = format_records(records, column_names)
    return jsonify(formatted_records)


@app.route('/addcliente', methods=['POST'])
def add_client():
    nombre = request.args.get('nombre')
    permisos = request.args.get('permisos')
    usuario = request.args.get('usuario')
    pswd_app = request.args.get('pswd_app')

    if not nombre or not permisos or not usuario or not pswd_app:
        return "Faltan datos", 400
    connex, cursor = db.get_connection_to_db()
    result = get_clients_with_par(usuario)
    if result:
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

    connex, cursor = db.get_connection_to_db()

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

    connex, cursor = db.get_connection_to_db()
    respuesta = get_clients_with_par(id)

    if respuesta:
        # Realizar la eliminación en la base de datos
        cursor.execute("DELETE FROM clientes WHERE id = %s", (id,))
        connex.commit()
        connex.close()
        return "Registro eliminado", 200  # Código de estado 200 OK

    else:
        return "No existe ese registro", 404


# Resto del código (sin cambios)

if __name__ == '__main__':
    app.run(host="0.0.0.0", port=3000)
