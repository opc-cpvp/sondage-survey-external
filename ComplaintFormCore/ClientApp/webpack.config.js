const path = require("path");

const config = {
    entry: "./src/index.ts",
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                use: "ts-loader",
                exclude: /node_modules/
            }
        ]
    },
    resolve: {
        extensions: [".tsx", ".ts", ".js"],
        alias: {
            // Since we need to compile templates on the client(e.g.passing a
            // string to the template option, or mounting to an element using
            // its in-DOM HTML as the template), we will need the compiler and
            // thus the full build
            // see: https://vuejs.org/v2/guide/installation.html#Webpack
            // https://stackoverflow.com/questions/49334815/vue-replaces-html-with-comment-when-compiling-with-webpack
            vue$: "vue/dist/vue.esm.js"
        }
    },
    output: {
        filename: "bundle.js",
        path: path.resolve(__dirname, "../wwwroot/dist")
    }
};

module.exports = (env, argv) => {
    if (argv.mode === 'development') {
        config.devtool = "inline-source-map";
    }

    return config;
}
