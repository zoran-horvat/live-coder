import { Dialogs } from "./dialogs";
import { Environment } from "./environment";

export abstract class Ide {
	abstract get dialogs(): Dialogs;
    abstract get environment(): Environment;

	abstract withNoEditorsOpen() : Promise<void>;
	abstract withWorkspaceOpen(fsPath: string) : Promise<void>;
	abstract withExplorerFoldersCollapsed() : Promise<void>;
	abstract withScriptEditorActive() : Promise<void>;
	abstract editDocument(path: string) : Promise<void>;
}