"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.VsCodeClient = void 0;
const ide_1 = require("../ide-integration/ide");
const dialogs_1 = require("./dialogs");
class VsCodeClient extends ide_1.Ide {
    get dialogs() { return new dialogs_1.VsCodeDialogs(); }
}
exports.VsCodeClient = VsCodeClient;
//# sourceMappingURL=vscodeclient.js.map