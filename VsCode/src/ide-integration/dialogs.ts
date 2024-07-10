export abstract class Dialogs {
	abstract selectDirectoryOrShowError(prompt: string, errorMessage: string, initialPath: string | undefined): Promise<string | undefined>;
}