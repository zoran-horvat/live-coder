export abstract class Dialogs {
	abstract selectDirectoryOrShowError(prompt: string, errorMessage: string, initialPath: string | undefined): Promise<string | undefined>;
	abstract showInformationMessage(message: string): Promise<void>;
	abstract showErrorMessage(message: string): Promise<void>;
}