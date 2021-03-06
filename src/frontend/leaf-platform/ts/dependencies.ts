export const dependencies = {
  services: {
    ThemeService: require('./services/ThemeService').ThemeService
  },
  views: {
    Login: require('./views/Login').Login,
    Main: require('./views/Main').Main
  },
  templates: {
    Login: require('./templates/login.ejs'),
    Main: require('./templates/main.ejs')
  }
};
