import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HelperComponent } from './helper.component';

describe('HelperComponent', () => {
  let component: HelperComponent;
  let fixture: ComponentFixture<HelperComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HelperComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HelperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component (positive test)', () => {
    expect(component).toBeTruthy();
  });

  it('should fail to create the component if a required dependency is missing (negative test)', () => {
    TestBed.resetTestingModule();
    TestBed.configureTestingModule({
      // Intentionally omitting declarations to simulate failure
    });

    expect(() => {
      TestBed.createComponent(HelperComponent);
    }).toThrowError();
  });
});
