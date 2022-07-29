#pragma once

#include <windows.h>

#include <stdint.h>

#include "mercuryhook/elisabeth.h"

enum {
    MERCURY_IO_OPBTN_TEST = 0x01,
    MERCURY_IO_OPBTN_SERVICE = 0x02,
    MERCURY_IO_OPBTN_COIN = 0x04,
};

enum {
    MERCURY_IO_GAMEBTN_VOL_UP = 0x01,
    MERCURY_IO_GAMEBTN_VOL_DOWN = 0x02,
};

typedef void (*mercury_io_touch_callback_t)(const bool *state);
/* Get the version of the Wacca IO API that this DLL supports. This
   function should return a positive 16-bit integer, where the high byte is
   the major version and the low byte is the minor version (as defined by the
   Semantic Versioning standard).

   The latest API version as of this writing is 0x0100. */

uint16_t mercury_io_get_api_version(void);

/* Initialize the IO DLL. This is the second function that will be called on
   your DLL, after mercury_io_get_api_version.

   All subsequent calls to this API may originate from arbitrary threads.

   Minimum API version: 0x0100 */

HRESULT mercury_io_init(void);

/* Send any queued outputs (of which there are currently none, though this may
   change in subsequent API versions) and retrieve any new inputs.

   Minimum API version: 0x0100 */

HRESULT mercury_io_poll(void);

/* Get the state of the cabinet's operator buttons as of the last poll. See
   MERCURY_IO_OPBTN enum above: this contains bit mask definitions for button
   states returned in *opbtn. All buttons are active-high.

   Minimum API version: 0x0100 */

void mercury_io_get_opbtns(uint8_t *opbtn);

/* Get the state of the cabinet's gameplay buttons as of the last poll. See
   MERCURY_IO_GAMEBTN enum above for bit mask definitions. Inputs are split into
   a left hand side set of inputs and a right hand side set of inputs: the bit
   mappings are the same in both cases.

   All buttons are active-high, even though some buttons' electrical signals
   on a real cabinet are active-low.

   Minimum API version: 0x0100 */

void mercury_io_get_gamebtns(uint8_t *gamebtn);

HRESULT mercury_io_touch_init(void);

void mercury_io_touch_start(mercury_io_touch_callback_t callback);

void mercury_io_touch_set_leds(struct led_data data);