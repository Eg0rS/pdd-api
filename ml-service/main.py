from config.app_config import AppConfig
from config.kafka_config import KafkaConfig
from config.minio_config import MinioConfig
from kafka.consumer import Consumer
from kafka.producer import Producer
from miniotest.miniotest import MinioClient


def main():
    config = AppConfig()
    kafka_config = config.get_kafka_config()
    minio_config = config.get_minio_config()

    minio_client = MinioClient(MinioConfig(minio_config))
    kafka_consumer = Consumer(KafkaConfig(kafka_config))
    kafka_producer = Producer(KafkaConfig(kafka_config))

    kafka_consumer.start_consume()


if __name__ == '__main__':
    main()
