<#import "template.ftl" as layout>

<@layout.pageHeader "">
    <head>
        <link rel="stylesheet" href="${url.resourcesPath}/css/login-otp.css">
    </head>
    <div class="login-wrapper">
        <div class="login-left">
            <img src="${url.resourcesPath}/img/login-illustration.png" alt="OTP Illustration" class="login-image">
        </div>
        <div class="login-right">
            <div class="title-container">
                <img src="${url.resourcesPath}/img/otp-icon.png" alt="OTP Icon" class="logo-image">
                <h1 class="title">OTP Verification</h1>
            </div>
            <form action="${url.loginAction}" method="post">
                <div class="input-group">
                    <input type="text" id="otp" name="otp" placeholder="Enter the authenticator code" required>
                </div>
                <button type="submit">Sign In</button>
            </form>
        </div>
    </div>
</@layout.pageHeader>