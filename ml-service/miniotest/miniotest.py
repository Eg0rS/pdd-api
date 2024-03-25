from minio import Minio

from config.minio_config import MinioConfig


class MinioClient:
    def __init__(self, config: MinioConfig):
        self.config = config
        self.client = Minio(
            self.config.get_endpoint(),
            access_key=self.config.get_access_key(),
            secret_key=self.config.get_secret_key(),
            secure=False
        )
        bucket = self.client.bucket_exists(config.get_bucket())

        if not bucket:
            self.client.make_bucket(config.get_bucket())
        else:
            print("Bucket requests already exists")


    def put_file(self, path):
        result = self.client.fput_object(self.config.get_bucket(), path)
        return result.etag

    def get_file(self, object_id):
        return self.client.get_object(self.config.get_bucket(), object_id)
