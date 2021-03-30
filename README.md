# 91crawler

小视频下载器, 你懂的

# Environment

- Linux
- GFW外, 或自行设置代理:

./hlsdl -p 127.0.0.1:8118  *** 

```cs
// 本地测试代理
session.Proxy = "127.0.0.1:8118";
```



# Requirements

- https://github.com/selsta/hlsdl
  编译后放入同级目录, 并给与执行权限

- .NET5 Runtime

# Usage

```shell
dotnet 91crawler.exe "http://xxxx.com/view_video.php?viewkey=zzzzzzzz"
```