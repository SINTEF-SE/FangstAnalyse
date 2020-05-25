class TopElement {

    constructor() {
        this._htmlElement = document.createElement('div');
        this._htmlElement.classList.add('fa-top-element');
        this.show(false)
    }

    text(text) {
        this._htmlElement.innerText = text;
    }

    element() {
        return this._htmlElement;
    }
    
    show(v) {
        v 
            ? this._htmlElement.setAttribute('style', 'display: flex;')
            : this._htmlElement.setAttribute('style', 'display: none;');
    }

}
