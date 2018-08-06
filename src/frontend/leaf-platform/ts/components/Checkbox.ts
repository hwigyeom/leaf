import { FormComponent } from './FormComponent';
import { IComponentOptions } from './IComponentOptions';

export interface ICheckboxOptions extends IComponentOptions {
  id: string;
  label?: string;
  checked?: boolean;
}

export class Checkbox extends FormComponent {

  constructor(options: ICheckboxOptions) {
    super();
    this._options = options;
  }

  private _$container: JQuery<Element>;

  public get $container(): JQuery<Element> {
    return this._$container;
  }

  private _$element: JQuery<Element>;

  public get $element(): JQuery<Element> {
    return this._$element;
  }

  private _options: ICheckboxOptions;

  public get options(): ICheckboxOptions {
    return this._options;
  }

  public get id(): string {
    return this._options.id;
  }

  public get checked(): boolean {
    return this.$element.find('input[type=checkbox]').prop('checked');
  }

  public set checked(checked: boolean) {
    this.$element.find('input[type=checkbox').prop('checked', checked);
  }

  public async initialize(): Promise<void> {
    return new Promise<void>((resolve) => {
      this.createElement();
      resolve();
    });
  }

  public parseOptions(dummy: Element): ICheckboxOptions {
    /*
     * <leaf:checkbox id="id" checked="true">라벨</leaf:checkbox>
     */
    return {
      id: dummy.getAttribute('id'),
      label: dummy.innerHTML,
      checked: /true/i.test(dummy.getAttribute('checked'))
    };
  }

  public setOptions(options: ICheckboxOptions): void {
    this._options = options;
  }

  private createElement(): void {
    const $container = $('<span class="lf-checkbox-wrapper"></span>');
    const $input = $('<input type="checkbox" class="lf-checkbox">');

    const id = this.options.id;
    if (!id) {
      setTimeout(() => {
        throw new Error('leaf:checkbox - id 가 지정되지 않앗습니다.');
      });
    }

    $input.attr('id', this.id);
    $input.prop('checked', this._options.checked || false);
    $input.attr('data-role', 'leaf-component')
      .data('component', this);
    $container.append($input);

    const $label = $('<label></label>');
    $label.text(this.options.label);
    $label.attr('for', this._options.id);
    $container.append($label);

    this._$container = $container;
    this._$element = $input;
  }
}
