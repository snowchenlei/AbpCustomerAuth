var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Build, Component, Element, Host, Prop, State, Watch, getMode, h } from '@stencil/core';
import { getSvgContent } from './request';
import { getName, getUrl } from './utils';
/**
 * @virtualProp {"ios" | "md"} mode - The mode determines which platform styles to use.
 */
let Icon = class Icon {
    constructor() {
        this.mode = getIonMode(this);
        this.isVisible = false;
        /**
         * If enabled, ion-icon will be loaded lazily when it's visible in the viewport.
         * Default, `false`.
         */
        this.lazy = false;
    }
    connectedCallback() {
        // purposely do not return the promise here because loading
        // the svg file should not hold up loading the app
        // only load the svg if it's visible
        this.waitUntilVisible(this.el, '50px', () => {
            this.isVisible = true;
            this.loadIcon();
        });
    }
    disconnectedCallback() {
        if (this.io) {
            this.io.disconnect();
            this.io = undefined;
        }
    }
    waitUntilVisible(el, rootMargin, cb) {
        if (Build.isBrowser && this.lazy && typeof window !== 'undefined' && window.IntersectionObserver) {
            const io = this.io = new window.IntersectionObserver((data) => {
                if (data[0].isIntersecting) {
                    io.disconnect();
                    this.io = undefined;
                    cb();
                }
            }, { rootMargin });
            io.observe(el);
        }
        else {
            // browser doesn't support IntersectionObserver
            // so just fallback to always show it
            cb();
        }
    }
    loadIcon() {
        if (Build.isBrowser && this.isVisible) {
            const url = getUrl(this);
            if (url) {
                getSvgContent(url)
                    .then(svgContent => this.svgContent = svgContent);
            }
        }
        if (!this.ariaLabel) {
            const label = getName(this.name, this.icon, this.mode, this.ios, this.md);
            // user did not provide a label
            // come up with the label based on the icon name
            if (label) {
                this.ariaLabel = label
                    .replace('ios-', '')
                    .replace('md-', '')
                    .replace(/\-/g, ' ');
            }
        }
    }
    render() {
        const mode = this.mode || 'md';
        const flipRtl = this.flipRtl || (this.ariaLabel && this.ariaLabel.indexOf('arrow') > -1 && this.flipRtl !== false);
        return (h(Host, { role: "img", class: Object.assign(Object.assign({ [mode]: true }, createColorClasses(this.color)), { [`icon-${this.size}`]: !!this.size, 'flip-rtl': !!flipRtl && this.el.ownerDocument.dir === 'rtl' }) }, ((Build.isBrowser && this.svgContent)
            ? h("div", { class: "icon-inner", innerHTML: this.svgContent })
            : h("div", { class: "icon-inner" }))));
    }
};
__decorate([
    Element()
], Icon.prototype, "el", void 0);
__decorate([
    State()
], Icon.prototype, "svgContent", void 0);
__decorate([
    State()
], Icon.prototype, "isVisible", void 0);
__decorate([
    Prop()
], Icon.prototype, "color", void 0);
__decorate([
    Prop({ mutable: true, reflectToAttr: true })
], Icon.prototype, "ariaLabel", void 0);
__decorate([
    Prop()
], Icon.prototype, "ios", void 0);
__decorate([
    Prop()
], Icon.prototype, "md", void 0);
__decorate([
    Prop()
], Icon.prototype, "flipRtl", void 0);
__decorate([
    Prop()
], Icon.prototype, "name", void 0);
__decorate([
    Prop()
], Icon.prototype, "src", void 0);
__decorate([
    Prop()
], Icon.prototype, "icon", void 0);
__decorate([
    Prop()
], Icon.prototype, "size", void 0);
__decorate([
    Prop()
], Icon.prototype, "lazy", void 0);
__decorate([
    Watch('name'),
    Watch('src'),
    Watch('icon')
], Icon.prototype, "loadIcon", null);
Icon = __decorate([
    Component({
        tag: 'ion-icon',
        assetsDir: 'svg',
        styleUrl: 'icon.css',
        shadow: true
    })
], Icon);
export { Icon };
const getIonMode = (ref) => {
    return getMode(ref) || document.documentElement.getAttribute('mode') || 'md';
};
const createColorClasses = (color) => {
    return (color) ? {
        'ion-color': true,
        [`ion-color-${color}`]: true
    } : null;
};
