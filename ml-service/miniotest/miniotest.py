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
        bucket_requests = self.client.bucket_exists(config.get_bucket_requests())
        bucket_resolution = self.client.bucket_exists(config.get_bucket_resolution())
        if not bucket_requests:
            self.client.make_bucket(config.get_bucket_requests())
        else:
            print("Bucket requests already exists")
        if not bucket_resolution:
            self.client.make_bucket(config.get_bucket_resolution())
        else:
            print("Bucket resolution already exists")

    def put_file(self, path):
        result = self.client.fput_object(self.config.get_bucket_resolution(), path)
        return result.etag

    def get_file(self, object_id):
        return self.client.get_object(self.config.get_bucket_requests(), object_id)
