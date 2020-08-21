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
    ],
    rules: {
        "@typescript-eslint/no-unsafe-call": 1,
        "@typescript-eslint/no-unsafe-member-access": 1,
        // "max-lines-per-function": ["error", {
        //     "max": 30,
        //     "skipBlankLines": true,
        //     "skipComments": true
        // }]
        "max-statements": ["error", { "max": 10 }]
    }
};
