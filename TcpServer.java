import java.io.*;
import java.net.*;

public class TcpServer {
    public static void main(String[] args) {
        int cong = 8080; // bạn có thể đổi thành 9000 nếu muốn
        try (ServerSocket serverSocket = new ServerSocket(cong)) {
            System.out.println("✅ Máy chủ đang chạy trên cổng " + cong);
            System.out.println("Nhập 'Bye' để thoát.");

            Socket socket = serverSocket.accept();
            System.out.println("📡 Có kết nối từ " + socket.getInetAddress());

            BufferedReader in = new BufferedReader(new InputStreamReader(socket.getInputStream()));
            PrintWriter out = new PrintWriter(socket.getOutputStream(), true);

            String nhan;
            while ((nhan = in.readLine()) != null) {
                System.out.println("📨 Nhận từ client: " + nhan);
                if (nhan.equalsIgnoreCase("bye")) {
                    System.out.println("🛑 Kết thúc kết nối.");
                    break;
                }
                out.println("Server đã nhận: " + nhan);
            }

            socket.close();
        } catch (IOException e) {
            System.out.println("Lỗi: " + e.getMessage());
        }
    }
}
