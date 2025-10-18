import java.io.*;
import java.net.*;

public class TcpClient {
    public static void main(String[] args) {
        String diaChi = "localhost";
        int cong = 8080; // nhớ trùng với server nha

        try (Socket socket = new Socket(diaChi, cong);
             BufferedReader nhapTuBanPhim = new BufferedReader(new InputStreamReader(System.in));
             PrintWriter out = new PrintWriter(socket.getOutputStream(), true);
             BufferedReader in = new BufferedReader(new InputStreamReader(socket.getInputStream()))) {

            System.out.println("✅ Kết nối tới server thành công.");
            System.out.println("Nhập tin nhắn (gõ 'Bye' để thoát):");

            String duLieuGui;
            while ((duLieuGui = nhapTuBanPhim.readLine()) != null) {
                out.println(duLieuGui);
                if (duLieuGui.equalsIgnoreCase("bye")) {
                    System.out.println("👋 Đã ngắt kết nối.");
                    break;
                }
                String phanHoi = in.readLine();
                System.out.println("💬 Phản hồi từ server: " + phanHoi);
            }

        } catch (IOException e) {
            System.out.println("Lỗi: " + e.getMessage());
        }
    }
}
