"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Integration = void 0;
const localfilesystem_1 = require("./fs-integration/localfilesystem");
const vscodeclient_1 = require("./vscode-integration/vscodeclient");
class Integration {
    static ideInstance;
    static fsInstanced;
    static get ide() {
        if (!Integration.ideInstance) {
            Integration.ideInstance = new vscodeclient_1.VsCodeClient();
        }
        return Integration.ideInstance;
    }
    static get fs() {
        if (!Integration.fsInstanced) {
            Integration.fsInstanced = new localfilesystem_1.LocalFileSystem();
        }
        return Integration.fsInstanced;
    }
}
exports.Integration = Integration;
//# sourceMappingURL=integration.js.map