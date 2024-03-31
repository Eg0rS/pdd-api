import random

def encode_message(message, gamma):
    encoded_message = b''
    for i in range(len(message)):
        encoded_byte = message[i] ^ gamma[i % len(gamma)]
        encoded_message += bytes([encoded_byte])
    return encoded_message

# Исходное сообщение в UTF-16
original_message = "Пример сообщения".encode('utf-8')

# Создание гаммы
gamma = bytes([random.randint(0, 255) for _ in range(len(original_message))])

# Кодирование сообщения с использованием гаммы
encoded_message = encode_message(original_message, gamma).decode(encoding='utf-8')

print("Закодированное сообщение в UTF-16 с использованием 8-битной гаммы:")
print(encoded_message)