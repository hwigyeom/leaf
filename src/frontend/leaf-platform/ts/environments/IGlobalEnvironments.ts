export interface IGlobalEnvironments {
  culture: string;
  title: string;
  subTitle?: string;
  theme?: string;
  keys: { token: string, sid?: string };
}
