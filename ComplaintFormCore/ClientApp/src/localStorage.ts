import { IStorage } from "./storage";

export class LocalStorage implements IStorage {
    // eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
    save(key: string, value: any): void {
        window.localStorage.setItem(key, JSON.stringify(value));
    }

    load(key: string): any {
        const value = window.localStorage.getItem(key);

        if (value === null) {
            return value;
        }

        // eslint-disable-next-line @typescript-eslint/no-unsafe-return
        return JSON.parse(value);
    }

    remove(key: string): void {
        window.localStorage.removeItem(key);
    }
}
