# Classe que representa l'entitat User
class User:
    __table_name__ = "users_data"
    __keys_user__ = ['id', 'name', 'role', 'password', 'gym_id', 'user_name', 'log']

    def __init__(self, id: int, name: str, user_name: str, pswd_app: str, rol_user: str, gym_id: int, log: int):
        self.__id = id
        self.__name = name
        self.__rol_user = rol_user
        self.__pswd_app = pswd_app
        self.__gym_id = gym_id
        self.__user_name = user_name
        self.__log = log

    # GETTERS
    @property
    def id(self):
        return self.__id

    @property
    def name(self):
        return self.__name

    @property
    def user_name(self):
        return self.__user_name

    @property
    def pswd_app(self):
        return self.__pswd_app

    @property
    def rol_user(self):
        return self.__rol_user

    @property
    def gym_id(self):
        return self.__gym_id

    @property
    def log(self):
        return self.__log

    # SETTERS
    @name.setter
    def name(self, new_name: str) -> str:
        self.__name = new_name
        return self.__name

    @pswd_app.setter
    def pswd_app(self, new_pswd_app):
        self.__pswd_app = new_pswd_app
        return self.__pswd_app
    @rol_user.setter
    def rol_user(self, new_rol_user):
        self.__rol_user = new_rol_user
        return self.__rol_user
    @log.setter
    def log(self, new_log):
        self.__log = new_log
        return self.__log


# Classe que representa l'entitat Gym
class Gym:
    __table_name__ = "gym"
    __keys_gym__ = ['id', 'name', 'address', 'phone_number', 'schedule']

    def __init__(self, id: int, name: str, address: str, phone_number: str, schedule: list):
        self.__id = id
        self.__name = name
        self.__address = address
        self.__phone_number = phone_number
        self.__schedule = schedule

    # GETTERS
    def get_id(self):
        return self.__id

    def get_name(self):
        return self.__name

    def get_address(self):
        return self.__address

    def get_phone_number(self):
        return self.__phone_number

    def get_schedule(self):
        return self.__schedule

    # SETTERS
    def set_name(self, new_name):
        self.__name = new_name
        return self.__name

    def set_addres(self, new_address):
        self.__address = new_address
        return self.__address

    def set_phone_number(self, new_phone_number):
        self.__phone_number = new_phone_number
        return self.__phone_number

    def set_schedule(self, new_schedule):
        self.__schedule = new_schedule
        return self.__schedule


# Classe que representa l'entitat GymEvent
class GymEvent:
    __table_name__ = "gym_events"

    __keys_events__ = ['id', 'name', 'whereisit', 'schedule', 'qty_max_attendes', 'qty_got_it', 'user_id', 'gym_id', 'done','date', 'hour']

    def __init__(self, id: int, name: str, whereisit: str, schedule: str, qty_max_attendes: int, qty_got_it: int,
                 rating: int, user_id: int, gym_id: int, done: bool):
        self.__id = id
        self.__name = name
        self.__whereisit = whereisit
        self.__schedule = schedule
        self.__qty_max_attendes = qty_max_attendes
        self.__qty_got_it = qty_got_it
        self.__rating = rating
        self.__user_id = user_id
        self.__gym_id = gym_id
        self.__done = done

    # GETTERS
    def get_id(self):
        return self.__id

    def get_name(self):
        return self.__name

    def get_whereisit(self):
        return self.__whereisit

    def get_schedule(self):
        return self.__schedule

    def get_qty_max_attendes(self):
        return self.__qty_max_attendes

    def get_qty_got_it(self):
        return self.__qty_got_it

    def get_rating(self):
        return self.__rating

    def get_user_id(self):
        return self.__user_id

    def get_gym_id(self):
        return self.__gym_id

    def get_done(self):
        return self.__done

    # SETTERS
    def set_name(self, new_name):
        self.__name = new_name
        return self.__name

    def set_whereisit(self, new_whereisit):
        self.__name = new_whereisit
        return self.__whereisit

    def set_schedule(self, new_schedule):
        self.__schedule = new_schedule
        return self.__schedule

    def set_qty_max_attendes(self, new_qty_max_attendes):
        self.__qty_max_attendes = new_qty_max_attendes
        return self.__qty_max_attendes

    def set_qty_got_it(self, new_qty_got_it):
        self.__qty_got_it = new_qty_got_it
        return self.__qty_got_it

    def set_rating(self, new_rating):
        self.__rating = new_rating
        return self.__rating

    def set_done(self, new_done):
        self.__done = new_done
        return self.__done


# Classe que representa l'entitat Activity
class Activity:
    __table_name__ = "activities"
    __keys_user__ = ['id', 'gym_id', 'name', 'qty_max_attendes', 'qty_got_it', 'schedule']

    def __init__(self, id: int, id_gym: int, name: str, qty_max_attendes: int, qty_got_it: int, schedule: list[str]):
        self.__id = id
        self.__id_gym = id_gym
        self.__name = name
        self.__qty_max_attendes = qty_max_attendes
        self.__qty_got_it = qty_got_it
        self.__schedule = schedule

    # GETTERS
    def get_id(self):
        return self.__id

    def get_id_gym(self):
        return self.__id_gym

    def get_name(self):
        return self.__name

    def get_gty_max_attendes(self):
        return self.__qty_max_attendes

    def get_qty_got_it(self):
        return self.__qty_got_it

    def get_schedule(self):
        return self.__schedule

    # SETTERS
    def set_name__(self, new_name):
        self.__name = new_name
        return self.__name

    def set_qty_max_attendes(self, new_max):
        self.__qty_max_attendes = new_max
        return self.__qty_max_attendes

    def set_qty_got_it(self, new_qty):
        self.__qty_got_it = new_qty
        return self.__qty_got_it

    def set_schedule(self, new_schedule):
        self.__schedule = new_schedule
        return self.__schedule
