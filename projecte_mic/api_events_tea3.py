from flask import Flask, request, jsonify

from database_models_tea2 import User, GymEvent
from utils_tea_2 import Connexion

app = Flask(__name__)
db = Connexion()


#  __keys_events__ = ['id', 'name', 'whereisit', 'schedule', 'qty_max_attendes', 'qty_got_it', 'rating', 'user_id',
#                      'gym_id', 'done']

@app.route('/obtener_eventos')
def get_all_events():
    token = request.headers.get('Authorization')
    rol_user, id, user_name, gym_name = db.validate_rol_user(token)
    if rol_user == 'admin':
        results = db.get_elements_filtered(id, GymEvent.__table_name__, 'gym_id', '*')
        if results:
            print(type(results))
            results_dict = [dict(zip(GymEvent.__keys_events__, row)) for row in results]
            return jsonify({results_dict}), 200
        else:
            return jsonify({'message': 'No es possible recuperar les dades'}), 404
    elif rol_user == 'normal':
        user_id = db.get_elements_filtered(user_name, User.__table_name__, "user_name", "id")[0][0]
        results = db.get_elements_filtered(user_id, GymEvent.__table_name__, "user_id", "*")
        if results:
            results_dict = [dict(zip(GymEvent.__keys_events__, row)) for row in results]
            return jsonify({results_dict}), 200
        else:
            return jsonify({'message': 'No es possible recuperar les dades'}), 404
    else:
        return jsonify({'message': 'No tens permisos per fer la gestió'}), 401


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=6000)
