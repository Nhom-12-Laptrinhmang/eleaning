import socket
import sys


def run_server():
    """Chạy TCP Server: Lắng nghe, nhận 'Hello World', gửi phản hồi và thoát."""
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)  # Cho phép reuse port nếu cần
    server.bind(('localhost', 12345))
    server.listen(1)
    print("Server đang lắng nghe trên localhost:12345...")

    conn, addr = server.accept()
    print(f"Kết nối từ {addr}")

    data = conn.recv(1024).decode('utf-8').strip()
    if data == "Hello World":
        response = "Đã nhận Hello World"
        conn.send(response.encode('utf-8'))
        print(f"Đã gửi phản hồi: {response}")
    else:
        print(f"Dữ liệu nhận được không khớp: {data}")

    conn.close()
    server.close()
    print("Server đã thoát.")


def run_client():
    """Chạy TCP Client: Kết nối, gửi 'Hello World', nhận phản hồi và thoát."""
    try:
        client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        client.connect(('localhost', 12345))

        message = "Hello World"
        client.send(message.encode('utf-8'))
        print(f"Đã gửi: {message}")

        response = client.recv(1024).decode('utf-8')
        print(f"Phản hồi từ Server: {response}")

        client.close()
        print("Client đã thoát.")
    except ConnectionRefusedError:
        print("Lỗi: Không thể kết nối đến Server. Hãy chạy Server trước!")
    except Exception as e:
        print(f"Lỗi: {e}")


if __name__ == "__main__":
    if len(sys.argv) != 2:
        print("Hướng dẫn sử dụng:")
        print("  python echo_tcp.py server   # Chạy Server")
        print("  python echo_tcp.py client   # Chạy Client (sau khi Server đang chạy)")
        sys.exit(1)

    mode = sys.argv[1].lower()
    if mode == "server":
        run_server()
    elif mode == "client":
        run_client()
    else:
        print("Lỗi: Argument không hợp lệ. Sử dụng 'server' hoặc 'client'.")
        sys.exit(1)
