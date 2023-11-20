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

    def __init__(self, id: int, name: str, address: str, phone_number: str, schedule: list[str]):
        self.__id = id
        self.__name = name
        self.__address = address
        self.__phone_number = phone_number
        self.__schedule = schedule

    # GETTERS
    @property
    def id(self):
        return self.__id

    @property
    def name(self):
        return self.__name

    @property
    def address(self):
        return self.__address

    @property
    def phone_number(self):
        return self.__phone_number

    @property
    def schedule(self):
        return self.__schedule

    # SETTERS
    @name.setter
    def name(self, new_name):
        self.__name = new_name
        return self.__name

    @address.setter
    def address(self, new_address):
        self.__address = new_address
        return self.__address

    @phone_number.setter
    def phone_number(self, new_phone_number):
        self.__phone_number = new_phone_number
        return self.__phone_number

    @schedule.setter
    def schedule(self, new_schedule: list[str]) -> list[str]:
        self.__schedule = new_schedule
        return self.__schedule

#TODO MODIFICAR ESTA CLASSE, LAS COLUMNAS NO ESTAN BIEN.
# Classe que representa l'entitat GymEvent
class GymEvent:
    __table_name__ = "gym_events"

    __keys_events__ = ['id', 'name', 'whereisit', 'qty_max_attendes', 'qty_got_it', 'user_id', 'gym_id',
                       'done', 'date', 'hour']

    def __init__(self, id: int, name: str, whereisit: str, qty_max_attendes: int, qty_got_it: int
                 , user_id: int, gym_id: int, done: bool, date, hour):
        self.__id = id
        self.__name = name
        self.__whereisit = whereisit
        self.__qty_max_attendes = qty_max_attendes
        self.__qty_got_it = qty_got_it
        self.__user_id = user_id
        self.__gym_id = gym_id
        self.__done = done
        self.__date = date
        self.__hour = hour

    # GETTERS
    @property
    def id(self):
        return self.__id

    @property
    def name(self):
        return self.__name

    @property
    def whereisit(self):
        return self.__whereisit

    @property
    def qty_max_attendes(self):
        return self.__qty_max_attendes

    @property
    def qty_got_it(self):
        return self.__qty_got_it

    @property
    def user_id(self):
        return self.__user_id

    @property
    def gym_id(self):
        return self.__gym_id

    @property
    def done(self):
        return self.__done

    @property
    def date(self):
        return self.__date

    @property
    def hour(self):
        return self.__hour

    # SETTERS
    @name.setter
    def name(self, new_name):
        self.__name = new_name
        return self.__name

    @whereisit.setter
    def whereisit(self, new_whereisit):
        self.__name = new_whereisit
        return self.__whereisit

    @qty_max_attendes.setter
    def qty_max_attendes(self, new_qty_max_attendes):
        self.__qty_max_attendes = new_qty_max_attendes
        return self.__qty_max_attendes

    @qty_got_it.setter
    def qty_got_it(self, new_qty_got_it):
        self.__qty_got_it = new_qty_got_it
        return self.__qty_got_it

    @done.setter
    def done(self, new_done):
        self.__done = new_done
        return self.__done

    @date.setter
    def date(self, new_date):
        self.__date = new_date
        return self.__date

    @hour.setter
    def hour(self, new_hour):
        self.__hour = new_hour
        return self.__hour


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
    @property
    def id(self):
        return self.__id

    @property
    def id_gym(self):
        return self.__id_gym

    @property
    def name(self):
        return self.__name

    @property
    def gty_max_attendes(self):
        return self.__qty_max_attendes

    @property
    def qty_got_it(self):
        return self.__qty_got_it

    @property
    def schedule(self):
        return self.__schedule

    # SETTERS
    @name.setter
    def name(self, new_name):
        self.__name = new_name
        return self.__name

    @gty_max_attendes.setter
    def qty_max_attendes(self, new_max):
        self.__qty_max_attendes = new_max
        return self.__qty_max_attendes

    @qty_got_it.setter
    def qty_got_it(self, new_qty):
        self.__qty_got_it = new_qty
        return self.__qty_got_it

    @schedule.setter
    def schedule(self, new_schedule):
        self.__schedule = new_schedule
        return self.__schedule
