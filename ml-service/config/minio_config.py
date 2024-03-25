class MinioConfig(object):

    def __init__(self, dict):
        self.access_key = dict.get('access_key')
        self.secret_key = dict.get('secret_key')
        self.endpoint = dict.get('endpoint')
        self.bucket = dict.get('bucket')

    def get_access_key(self):
        return self.access_key

    def get_secret_key(self):
        return self.secret_key

    def get_endpoint(self):
        return self.endpoint

    def get_bucket(self):
        return self.bucket

