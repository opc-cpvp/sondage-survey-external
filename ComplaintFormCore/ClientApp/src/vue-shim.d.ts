// For some odd reason, TypeScript wasn't able to locate *.vue files such as '@/App.vue' in the index.ts file
// So I found this on Stackoverflow https://stackoverflow.com/questions/42002394/importing-vue-components-in-typescript-file
// and it works for now
declare module "*.vue" {
    import VueHack from "vue";
    export default VueHack;
}
