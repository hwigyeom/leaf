import { Textbox } from './Textbox';

export class Password extends Textbox {
  constructor() {
    super();
    this._inputType = 'password';
  }
}
