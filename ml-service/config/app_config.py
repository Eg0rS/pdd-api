import json
import os


class AppConfig:
    def __init__(self):
        env = os.environ['ENV']

        if env is None:
            env = 'Development'

        config_file = './.config/' + env + '.json'
        with open(config_file, 'r') as file:
            self.config = json.load(file)

    def get_kafka_config(self):
        return self.config.get('kafka')

    def get_minio_config(self):
        return self.config.get('minio')
