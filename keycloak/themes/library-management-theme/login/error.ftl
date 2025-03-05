<#import "template.ftl" as layout>

<@layout.pageHeader "An error occurred">
    <p>${kcSanitize(message.summary)?no_esc}</p>
    <a href="${url.loginUrl}"><button>Return to Login</button></a>
</@layout.pageHeader>