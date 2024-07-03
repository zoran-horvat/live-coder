import { Dialogs } from "./Dialogs";

export abstract class Ide
{
	abstract get dialogs(): Dialogs;

	abstract showError(message: string): void;
}