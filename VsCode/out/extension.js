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
exports.activate = activate;
exports.deactivate = deactivate;
const vscode = __importStar(require("vscode"));
const deploy = __importStar(require("./commands/deploy"));
const vscode_1 = require("./vscode/vscode");
function activate(context) {
    const commands = new Commands(Integration.getInstance());
    pushCommand(context, 'demo.deploy', commands.deploy);
}
function deactivate() { }
function pushCommand(context, command, callback) {
    const disposable = vscode.commands.registerCommand(command, callback);
    context.subscriptions.push(disposable);
}
async function executeCommand(f) {
    try {
        await f();
    }
    catch (error) {
        vscode.window.showErrorMessage(`Error: ${error instanceof Error ? error.message : String(error)}`);
    }
}
class Commands {
    ide;
    constructor(ide) {
        this.ide = ide;
    }
    get deploy() { return () => deploy.command(this.ide); }
}
class Integration {
    static instance;
    static getInstance() {
        if (!Integration.instance) {
            Integration.instance = new vscode_1.VsCode();
        }
        return Integration.instance;
    }
}
//# sourceMappingURL=extension.js.map