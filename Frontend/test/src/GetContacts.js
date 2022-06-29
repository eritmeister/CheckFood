import {useEffect, useState} from "react";
import axios from "axios";
import {Button, TextField} from "@material-ui/core";
import './index.css'


function ProductsFetching() {
    const [products, setProducts] = useState([])
    const [searchingProduct, setSearchingProduct] = useState("")

    function productCall(productName) {
        axios.get('https://localhost:7077/GetProductInfo?productName=' + productName, {
            headers: {
                'Access-Control-Allow-Origin': 'localhost:3001'
            }
        })
            .then(res => {
                console.log(res)
                setProducts(res.data)
            })
            .catch(err => {
                console.log(err)
            })
    }
    return (
        <div>
            <div class="Search">
            <TextField
                style = {{width: 400}}
                id="outlined-basic"
                label="Введите название продукта"
                variant="outlined"
                onKeyPress={(ev) => {
                    console.log(`Pressed keyCode ${ev.key}`);
                    if (ev.key === 'Enter') {
                        // Do code here
                        ev.preventDefault();
                        productCall(searchingProduct)
                    }
                }}
                onChange={(ev) => {
                    console.log(ev.target.value)
                    setSearchingProduct(ev.target.value)
                }}
            />
            <Button
                onClick={ev => productCall(searchingProduct)}
                variant="contained">
                Поиск
            </Button>
            </div>
            <div>
                {
                    products.map(contact => (
                        <ul>
                        <li key={contact.id}>{contact.shopName}</li>
                        <li>{String(Math.floor(contact.price / 100)) + '.' + String(contact.price % 100)} руб.</li>
                        <li>{contact.productName}</li>
                            { contact.picture &&
                                <img
                                    style={{maxWidth: 300, maxHeight: 300}}
                                    src={contact.picture}
                                    alt="new"
                                />
                            }
                        </ul>
                    ))
                }
            </div>
        </div>
    )
}

export default ProductsFetching
