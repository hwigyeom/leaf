import { ComponentBase } from './ComponentBase';

const convertibleDOMComponents: { [name: string]: { new(...args: any[]): ComponentBase } } = {
  checkbox: require('./Checkbox').Checkbox,
  textbox: require('./Textbox').Textbox,
  password: require('./Password').Password
};

export { convertibleDOMComponents };
