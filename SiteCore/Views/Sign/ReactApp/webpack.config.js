var path = require('path');

module.exports = {
    entry: "./SignApp/app.jsx", // входная точка - исходный файл
    output: {
        path: path.resolve( '../../../wwwroot/js/ReactApp'),     // путь к каталогу выходных файлов - папка public
        publicPath: '../../../wwwroot/js/ReactApp',
        filename: "Sign.js"       // название создаваемого файла
    },
    devServer: {
        historyApiFallback: true,
        port: 8090,
        open: true
    },
    optimization: {
        minimize: false
    },
    module: {
        rules: [   //загрузчик для jsx
            {
                test: /\.jsx?$/, // определяем тип файлов
                exclude: /(node_modules)/,  // исключаем из обработки папку node_modules
                loader: "babel-loader",   // определяем загрузчик
                options: {
                    presets: ["@babel/preset-env", "@babel/preset-react"],
                    plugins: ['@babel/plugin-transform-runtime']// используемые плагины
                }
            }
        ]
    }
}