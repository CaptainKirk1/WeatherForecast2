import React, { useState } from 'react';
import { Input, Button, Dropdown, DropdownItem } from 'reactstrap';

const LocationSelection = (props) => {
    let { setLocationValue, searchChange } = props

    const [searchValue, setSearchValue] = useState('')
    const [locationList, setLocationList] = useState([])

    const getLocationList = async () => {
        const response = await fetch(`locationList?search=${searchValue}`)
        const data = await response.json()
        setLocationList(data)
    }

    return (
        <React.Fragment>
            Please Enter some letters, and then, click on an item on the list...
            <Input type="text" onChange={event => {
                setSearchValue(event.target.value)
                if (searchValue.length > 2) { getLocationList() }
            }} />
            <Dropdown title="Dropdown">
                {locationList.map(item =>
                    <DropdownItem onClick={(event) => {
                        setLocationValue(event.target.value)
                        searchChange(false)
                    }}>{item}</DropdownItem>
                )}
            </Dropdown>
        </React.Fragment>
    )

}

export { LocationSelection }
