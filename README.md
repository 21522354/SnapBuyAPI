# Mobile Pervasive Backend

## Yêu cầu hệ thống (Prerequisites)

### Phần mềm bắt buộc

- **Docker**: Phiên bản `4.20.x` hoặc cao hơn  
- **Visual Studio 2022**: Dùng để điều chỉnh source code  
- **Git**: Dùng để clone source code  

---

## Cài đặt (Installation)

### 1. Clone repository

```bash
git clone https://github.com/21522354/SnapBuyAPI
```

### 2. Mở project bằng Visual Studio

### 3. Mở Docker để chuẩn bị tạo các image của các service

### 4. Vào từng folder service của project, chạy lệnh build image sau:

```bash
docker build -t <user_name>/<image_name> .
```

### 5. Tải file cấu hình Docker Compose tại liên kết sau:

[Download docker-compose file](https://drive.google.com/file/d/1jh6HNHGKTyDvFkv4bMk_rUwSrchcoCgF/view?usp=sharing)

### 6. Mở file `docker-compose.yml` trong thư mục đã tải về, thay thế các tên image bằng các image tương ứng đã build ở bước 4. Đồng thời, chỉnh sửa cấu hình đường dẫn trong file `nginx.conf` nếu cần.

### 7. Mở Command Prompt và chạy lệnh sau:

```bash
docker compose up -d
```

### 8. Kiểm tra API bằng cách gọi từ Postman
