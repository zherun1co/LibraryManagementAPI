<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Library Management Login</title>
    <link rel="stylesheet" href="${url.resourcesPath}/css/login.css">
</head>
<body>
    <div class="login-wrapper">
        <div class="login-left">
            <img src="${url.resourcesPath}/img/login-illustration.png" alt="Login Illustration" class="login-image">
        </div>
        <div class="login-right">
            <div class="title-container">
                <img src="${url.resourcesPath}/img/logo.png" alt="Logo" class="logo-image">
                <h1 class="title">Library Management</h1>
            </div>
            <form action="${url.loginAction}" method="post">
                <div class="input-group">
                    <input type="text" name="username" placeholder="Username or email" required>
                </div>
                <div class="input-group">
                    <input type="password" name="password" placeholder="Password" required>
                </div>
                <div class="button-container">
                    <button type="submit">Sign In</button>
                </div>
            </form>
        </div>
    </div>
</body>
</html>