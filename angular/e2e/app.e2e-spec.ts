import { ApiTestTemplatePage } from './app.po';

describe('ApiTest App', function() {
  let page: ApiTestTemplatePage;

  beforeEach(() => {
    page = new ApiTestTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});

