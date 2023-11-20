from flask import Flask, request, jsonify

from database_models_tea2 import User, GymEvent
from utils_tea_2 import Connexion

app = Flask(__name__)
db = Connexion()


#  __keys_events__ = ['id', 'name', 'whereisit', 'schedule', 'qty_max_attendes', 'qty_got_it', 'user_id',
#                      'gym_id', 'done','date', 'hour']

@app.route('/obtener_eventos', methods=['POST'])
def get_all_events():
    token = request.headers.get('Authorization')
    rol_user, id, user_name, gym_name = db.validate_rol_user(token)
    results = db.get_elements_filtered(id, GymEvent.__table_name__, 'gym_id', '*')
    if results:
        results_dict = [dict(zip(GymEvent.__keys_events__, row)) for row in results]
        return jsonify(results_dict), 200
    else:
        return jsonify({'message': 'No es possible recuperar les dades'}), 404

@app.route('/filtrar_eventos', methods=['POST'])
def get_filtered_events():
    token = request.headers.get('Authorization')
    rol_user, id, user_name, gym_name = db.validate_rol_user(token)
    user_name_params = request.args.get('user_name')
    id_gym_user_params = db.get_elements_filtered(user_name_params, User.__table_name__, 'user_name', 'gym_id')
    if rol_user == 'admin' and id_gym_user_params == id:
        id_user = db.get_elements_filtered(user_name_params, User.__table_name__, 'user_name', 'id')
        results = db.get_elements_filtered(id_user, GymEvent.__table_name__,'user_id', '*')
        print(results)

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=6000)
