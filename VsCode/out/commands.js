"use strict";
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    var desc = Object.getOwnPropertyDescriptor(m, k);
    if (!desc || ("get" in desc ? !m.__esModule : desc.writable || desc.configurable)) {
      desc = { enumerable: true, get: function() { return m[k]; } };
    }
    Object.defineProperty(o, k2, desc);
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.Commands = void 0;
const vscode = __importStar(require("vscode"));
const deploy = __importStar(require("./commands/deploy"));
const recorddemo = __importStar(require("./commands/recorddemo"));
const editscript = __importStar(require("./commands/editscript"));
class Commands {
    integration;
    constructor(integration) {
        this.integration = integration;
    }
    get ide() { return this.integration.ide; }
    get fs() { return this.integration.fs; }
    get environment() { return this.integration.environment; }
    get deploy() { return () => this.safe(() => deploy.command(this.ide, this.fs, this.environment)); }
    get record() { return () => this.safe(() => recorddemo.command(this.ide, this.fs, this.environment)); }
    get edit() { return () => this.safe(() => editscript.command(this.ide, this.fs, this.environment)); }
    async safe(f) {
        try {
            await f();
        }
        catch (error) {
            vscode.window.showErrorMessage(`Error: ${error instanceof Error ? error.message : String(error)}`);
        }
    }
}
exports.Commands = Commands;
//# sourceMappingURL=commands.js.map