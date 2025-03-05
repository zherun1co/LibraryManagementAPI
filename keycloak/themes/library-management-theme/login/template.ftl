<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>${msg("loginTitle")}</title>
    <link rel="stylesheet" href="${url.resourcesPath}/css/styles.css">
</head>
<body>
    <div class="login-container">
        <img src="${url.resourcesPath}/img/logo.png" alt="Logo" class="logo">

        <#macro pageHeader title>
            <h2>${title}</h2>
            <#nested> <!-- Permite contenido dentro del macro -->
        </#macro>
    </div>
</body>
</html>