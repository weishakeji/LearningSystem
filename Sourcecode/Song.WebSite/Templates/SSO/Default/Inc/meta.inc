  <meta charset="utf-8"/>
<meta name="viewport" content="initial-scale=1,maximum-scale=1,user-scalable=no" />
<meta name="viewport" content="width=device-width, initial-scale=1,maximum-scale=1.0, user-scalable=no" />
<meta name="apple-mobile-web-app-capable" content="yes" />
<meta name="apple-mobile-web-app-status-bar-style" content="black" />
<meta name="format-detection" content="telephone=yes" />
<meta name="format-detection" content="email=no" />
<script type="text/javascript" src="/Utilities/Scripts/webdom.js"></script>
<script type="text/javascript" src="../Scripts/Public.js"></script>
<style type="text/css">
     *[v-cloak] {
        text-align: left !important;
        color: #999;
        width: 300px !important;
        height: 150px !important;
        margin: 50px auto !important;
        padding: 50px 80px !important;
        overflow: hidden !important;
        display: flex;
        box-sizing: border-box;
        /*下边线的动画，用于显示预载条*/
        --vcloak-loading-color: rgb(224, 224, 224);
        --vcloak-loading-bg: rgb(255, 255, 255);
        border-width: 0px 0px 3px 0px;
        border-style: solid;
        animation: gradient_animation .3s infinite linear;
        border-image-slice: 1;
        border-image-width: 1;
        border-image-outset: 0;
        border-image-repeat: stretch;
    }

    *[v-cloak]>* {
        display: none !important;
    }

    *[v-cloak]::before {
        content: "/";
        display: block;
        width: 30px;
        height: 30px;
        line-height: 28px;
        text-align: center;
        font-size: 26px;
        animation: degree_rotation_360 0.25s linear infinite;
        transform-origin: center center;
    }

    *[v-cloak]::after {
        content: "loading ... ";
        font-size: 18px;
        margin: 5px 0px 0px 10px;
    }

    @keyframes degree_rotation_360 {
        from { transform: rotate(0deg); }
      to { transform: rotate(360deg); }
    }

    @keyframes gradient_animation {
        0% {
            border-image-source: linear-gradient(to right, var(--vcloak-loading-color) 0%, var(--vcloak-loading-bg) 10%, var(--vcloak-loading-bg) 100%);
        }

        25% {
            border-image-source: linear-gradient(to right, var(--vcloak-loading-color) 10%, var(--vcloak-loading-bg) 20%, var(--vcloak-loading-bg) 100%);
        }

        50% {
            border-image-source: linear-gradient(to right, var(--vcloak-loading-color) 50%, var(--vcloak-loading-bg) 50%, var(--vcloak-loading-bg) 100%);
        }

        75% {
            border-image-source: linear-gradient(to right, var(--vcloak-loading-color) 80%, var(--vcloak-loading-bg) 80%, var(--vcloak-loading-bg) 100%);
        }

        100% {
            border-image-source: linear-gradient(to right, var(--vcloak-loading-color) 90%, var(--vcloak-loading-bg) 100%);
        }
    }
</style>