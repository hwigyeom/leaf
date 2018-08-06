import { FormComponent } from './FormComponent';
import { IComponentOptions } from './IComponentOptions';

export interface ITextboxOptions extends IComponentOptions {
  id: string;
  placeholder?: string;
  text?: string;
  autocomplete?: boolean;
  autocapitalize?: boolean;
  autofocus?: boolean;
}

export class Textbox extends FormComponent {

  protected _inputType: string;
  private _options: kendo.data.ObservableObject;

  constructor() {
    super();

    this._inputType = 'text';
  }

  private _$container: JQuery<Element>;

  public get $container(): JQuery<Element> {
    return this._$container;
  }

  private _$element: JQuery<Element>;

  public get $element(): JQuery<Element> {
    return this._$element;
  }

  public get text(): string {
    return this._options.get('text');
  }

  public set text(val: string) {
    this._options.set('text', val);
  }

  public async initialize(): Promise<void> {
    return new Promise<void>((resolve) => {
      resolve();
    });
  }

  public parseOptions(dummy: Element): ITextboxOptions {
    /*
     * <leaf:Textbox id="id" placeholder="placeholder" text="My Text Value" />
     */
    return {
      id: dummy.getAttribute('id'),
      placeholder: dummy.getAttribute('placeholder'),
      text: dummy.getAttribute('text'),
      autocomplete: dummy.getAttribute('autocomplete') === 'true',
      autocapitalize: dummy.getAttribute('autocapitalize') === 'true',
      autofocus: dummy.getAttribute('autofocus') === 'true'
    };
  }

  public setOptions(options: ITextboxOptions): void {
    this._options = kendo.observable(options);
    this.render();

    this._options.bind('change', (e) => {
      switch (e.field) {
        case 'placeholder':
          this.setPlaceholder(this._options.get('placeholder'));
          break;
        case 'text':
          this.setText(this._options.get('text'));
          break;
        default:
          break;
      }
    });
  }

  public getOptions(): ITextboxOptions {
    return this._options.toJSON() as ITextboxOptions;
  }

  private render(): void {
    if (!this._$container) {
      this._$container = $('<span class="lf-textbox-wrapper"></span>');
    }

    const options = this._options.toJSON() as ITextboxOptions;

    this._$container.empty();
    this._$element = $(`<input type="${this._inputType}" class="lf-textbox">`);
    this._$element
      .attr('id', options.id)
      .attr('autocomplete', !options.autocomplete ? 'off' : null)
      .attr('autocapitalize', !options.autocapitalize ? 'off' : null)
      .attr('autofocus', options.autofocus ? 'autofocus' : null)
      .attr('data-role', 'leaf-component')
      .data('component', this)
      .on('focus', (e: JQuery.Event) => {
        setTimeout(() => {
          (this.$element.get(0) as HTMLInputElement).setSelectionRange(0, 999);
        }, 10);
      })
      .on('change', () => {
        this.text = this._$element.val() as string;
      });

    this.setPlaceholder(this._options.get('placeholder'));

    this._$container.append(this._$element);
  }

  private setPlaceholder(text: string): void {
    this._$element.attr('placeholder', text);
  }

  private setText(text: string): void {
    this._$element.val(text);
  }
}
