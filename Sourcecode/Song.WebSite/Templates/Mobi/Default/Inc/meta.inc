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

        border-width: 0px 0px 20px 0px;
        border-style: solid;
        animation: gradient_animation .3s infinite linear;
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
        font-weight: bold;
        font-size: 26px;
        animation: rotate_360 0.15s infinite linear;
        transform-origin: center center;
    }

    *[v-cloak]::after {
        content: "loading ... ";
        font-size: 18px;
        margin: 5px 0px 0px 10px;
    }

    @keyframes rotate_360 {
        0% {
            transform: rotate(0deg);
        }

        25% {
            transform: rotate(90deg);
        }

        50% {
            transform: rotate(180deg);
        }

        75% {
            transform: rotate(270deg);
        }

        100% {
            transform: rotate(360deg);
        }
    }

    @keyframes gradient_animation {
        0% {
            border-image: linear-gradient(to right, rgb(140, 197, 255) 0%, #eee 10%, #eee 100%) 1;

        }

        25% {
            border-image: linear-gradient(to right, rgb(140, 197, 255) 10%, #eee 20%, #eee 100%) 1;

        }

        50% {
            border-image: linear-gradient(to right, rgb(140, 197, 255) 50%, #eee 50%, #eee 100%) 1;

        }

        75% {
            border-image: linear-gradient(to right, rgb(140, 197, 255) 80%, #eee 80%, #eee 100%) 1;

        }

        100% {
            border-image: linear-gradient(to right, rgb(140, 197, 255) 90%, #eee 100%) 1;

        }
    }
</style>