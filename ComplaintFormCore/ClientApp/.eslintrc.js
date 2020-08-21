module.exports = {
    env: {
        browser: true
    },
    root: true,
    parser: "@typescript-eslint/parser",
    plugins: [
        "@typescript-eslint",
        "@typescript-eslint/tslint",
        "jsdoc",
        "import"
    ],
    parserOptions: {
        project: "tsconfig.json",
        sourceType: "module"
    },
    extends: [
        "eslint:recommended",
        "plugin:@typescript-eslint/recommended",
        "plugin:@typescript-eslint/recommended-requiring-type-checking"
    ]
};
